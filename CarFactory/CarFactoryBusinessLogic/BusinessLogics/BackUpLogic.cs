using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Xml.Serialization;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.StoragesContracts;

namespace CarFactoryBusinessLogic.BusinessLogics
{
    public class BackUpLogic : IBackUpLogic
    {
        private readonly IBackUpInfo backUpInfo;
        public BackUpLogic(IBackUpInfo backUpInfo)
        {
            this.backUpInfo = backUpInfo;
        }
        public void CreateBackUp(BackUpSaveBinidngModel model)
        {
            if (backUpInfo == null)
            {
                return;
            }
            try
            {
                var dirInfo = new DirectoryInfo(model.FolderName);
                if (dirInfo.Exists)
                {
                    foreach (FileInfo file in dirInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }
                string fileName = $"{model.FolderName}.zip";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                // берем сборку, чтобы от нее создавать объекты
                Assembly assem = backUpInfo.GetAssembly();
                // вытаскиваем список классов для сохранения
                var dbsets = backUpInfo.GetFullList();
                // берем метод для сохранения (из базвого абстрактного класса)
                MethodInfo method = GetType().GetTypeInfo().GetDeclaredMethod("SaveToFile");
                foreach (var set in dbsets)
                {
                    // создаем объект из класса для сохранения
                    var elem = assem.CreateInstance(set.PropertyType.GenericTypeArguments[0].FullName);
                    // генерируем метод, исходя из класса
                    MethodInfo generic = method.MakeGenericMethod(elem.GetType());
                    // вызываем метод на выполнение
                    generic.Invoke(this, new object[] { model.FolderName });
                }
                // архивируем
                ZipFile.CreateFromDirectory(model.FolderName, fileName);
                // удаляем папку
                dirInfo.Delete(true);
            }
            catch (Exception)
            {
                // делаем проброс
                throw;
            }
        }
        private void SaveToFile<T>(string folderName) where T : class, new()
        {
            var records = backUpInfo.GetList<T>();
            var obj = new T();
            var xmlSerializer = new XmlSerializer(typeof(List<T>));
            using var fs = new FileStream(string.Format("{0}/{1}.xml", folderName, obj.GetType().Name), FileMode.OpenOrCreate);
            xmlSerializer.Serialize(fs, records);
        }
    }
}
