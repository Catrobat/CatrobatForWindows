﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Catrobat.IDE.Core;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.Services.Storage;
using Catrobat.IDE.Core.UI.PortableUI;

namespace Catrobat.IDE.WindowsShared.Services.Storage
{
    public class StorageWindowsShared : IStorage
    {
        private static int _imageThumbnailDefaultMaxWidthHeight = 200;
        private readonly List<Stream> _openedStreams = new List<Stream>();
        private StorageFolder _baseFolder;

        public StorageWindowsShared(StorageLocation storageLocation = StorageLocation.Temp)
        {
            switch(storageLocation)
            {
                case StorageLocation.Local:
                    _baseFolder = ApplicationData.Current.LocalFolder;
                    break;

                case StorageLocation.Roaming:
                    _baseFolder = ApplicationData.Current.RoamingFolder;
                    break;

                case StorageLocation.Temp:
                    _baseFolder = ApplicationData.Current.TemporaryFolder;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("storagelocation");
            }
        }

        //#region Synchron

        //#region File manipulation

        //public string[] GetFileNames(string path)
        //{
        //    var task = GetFileNamesAsync(path);
        //    task.Wait();
        //    return task.Result;
        //}

        //public bool FileExists(string path)
        //{
        //    var task = FileExistsAsync(path);
        //    task.Wait();
        //    return task.Result;
        //}

        //public Stream OpenFile(string path, StorageFileMode mode, StorageFileAccess access)
        //{
        //    var task = OpenFileAsync(path, mode, access);
        //    task.Wait();
        //    return task.Result;
        //}

        //public void DeleteFile(string path)
        //{
        //    DeleteFileAsync(path).Wait();
        //}

        //public void MoveFile(string sourcePath, string destinationPath)
        //{
        //    MoveFileAsync(sourcePath, destinationPath).Wait();
        //}

        //public void CopyFile(string sourcePath, string destinationPath)
        //{
        //    CopyFileAsync(sourcePath, destinationPath).Wait();
        //}

        //#endregion

        //#region Directory manipulation

        //public string[] GetDirectoryNames(string path)
        //{
        //    var task = GetDirectoryNamesAsync(path);
        //    task.Wait();
        //    return task.Result;
        //}

        //public bool DirectoryExists(string path)
        //{
        //    var task = DirectoryExistsAsync(path);
        //    task.Wait();
        //    return task.Result;
        //}

        //public void CreateDirectory(string path)
        //{
        //    CreateDirectoryAsync(path).Wait();
        //}

        //public void DeleteDirectory(string path)
        //{
        //    DeleteDirectoryAsync(path).Wait();
        //}

        //public void MoveDirectory(string sourcePath, string destinationPath)
        //{
        //    MoveDirectoryAsync(sourcePath, destinationPath).Wait();
        //}

        //public void CopyDirectory(string sourcePath, string destinationPath)
        //{
        //    CopyDirectoryAsync(sourcePath, destinationPath).Wait();
        //}

        //public void RenameDirectory(string directoryPath, string newDirectoryName)
        //{
        //    RenameDirectoryAsync(directoryPath, newDirectoryName).Wait();
        //}

        //#endregion

        //#region Specialized reading and writing

        //public object ReadSerializableObject(string path, Type type)
        //{
        //    var task = ReadSerializableObjectAsync(path, type);
        //    task.Wait();
        //    return task.Result;
        //}

        //public void WriteSerializableObject(string path, object serializableObject)
        //{
        //    WriteSerializableObjectAsync(path, serializableObject).Wait();
        //}

        //public string ReadTextFile(string path)
        //{
        //    var task = ReadTextFileAsync(path);
        //    task.Wait();
        //    return task.Result;
        //}

        //public void WriteTextFile(string path, string content)
        //{
        //    WriteTextFileAsync(path, content).Wait();
        //}

        //#endregion

        //#region Image reading and writing
        //public PortableImage LoadImage(string pathToImage)
        //{
        //    var task = LoadImageAsync(pathToImage);
        //    task.Wait();
        //    return task.Result;
        //}

        //public void SaveImage(string path, PortableImage image, bool deleteExisting, ImageFormat format)
        //{
        //    SaveImageAsync(path, image, deleteExisting, format).Wait();
        //}

        //public void DeleteImage(string pathToImage)
        //{
        //    DeleteImageAsync(pathToImage).Wait();
        //}

        //public PortableImage LoadImageThumbnail(string pathToImage)
        //{
        //    var task = LoadImageThumbnailAsync(pathToImage);
        //    task.Wait();
        //    return task.Result;
        //}

        //public void TryCreateThumbnail(string file)
        //{
        //    var task = TryCreateThumbnailAsync(file);
        //    task.Wait();
        //}

        //#endregion

        //#endregion

        #region Async

        #region File manipulation

        public async Task<string[]> GetFileNamesAsync(string path)
        {
            StorageFolder rootDirectory = await GetFolderAsync(path);
            var files = await rootDirectory.GetFilesAsync();

            return files.Select(file => file.Name).ToArray();
        }

        public async Task<bool> FileExistsAsync(string path)
        {
            StorageFile file = await GetFileAsync(path, false);
            return file != null;
        }

        public async Task<Stream> OpenFileAsync(string path, StorageFileMode mode, StorageFileAccess access)
        {
            var folderPath = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var accessMode = FileAccessMode.ReadWrite;

            switch (access)
            {
                case StorageFileAccess.Read:
                    accessMode = FileAccessMode.Read;
                    break;
                case StorageFileAccess.ReadWrite:
                    accessMode = FileAccessMode.ReadWrite;
                    break;
                case StorageFileAccess.Write:
                    accessMode = FileAccessMode.ReadWrite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("access");
            }
            StorageFile file;
            switch (mode)
            {
                case StorageFileMode.Append:
                case StorageFileMode.Open:
                case StorageFileMode.OpenOrCreate:
                case StorageFileMode.Truncate:
                    file = await GetFileAsync(path, false);
                    break;
                case StorageFileMode.Create:
                case StorageFileMode.CreateNew:
                    {
                        StorageFolder folder = await CreateFolderPathAsync(folderPath);
                        file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }

            if (file == null)
                return null;

            var stream = await file.OpenAsync(accessMode);
            _openedStreams.Add(stream.AsStream());
            return stream.AsStream();
        }
        
        public async Task DeleteFileAsync(string path)
        {
            StorageFile file = await GetFileAsync(path, false);

            if (file != null)
                await file.DeleteAsync();
        }

        public async Task MoveFileAsync(string sourcePath, string destinationPath)
        {
            StorageFile file = await GetFileAsync(sourcePath, false);
            if (file == null)
                return;

            var destinationFolderPath = Path.GetPathRoot(destinationPath);
            var destinationFolder = await GetFolderAsync(destinationFolderPath);

            await file.MoveAsync(destinationFolder);
        }

        public async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            StorageFile file = await GetFileAsync(sourcePath, false);
            if (file == null)
                return;

            var destinationFolderPath = Path.GetDirectoryName(destinationPath);
            var destinationFolder = await GetFolderAsync(destinationFolderPath);

            if(destinationFolder == null)
            {
                await CreateFolderPathAsync(destinationFolderPath);
                destinationFolder = await GetFolderAsync(destinationFolderPath);
            }
            
            var newFileName = Path.GetFileName(destinationPath);
            await file.CopyAsync(destinationFolder, newFileName, NameCollisionOption.ReplaceExisting);
        }

        #endregion

        #region Directory manipulation

        public async Task<string[]> GetDirectoryNamesAsync(string path)
        {
            StorageFolder rootDirectory = await GetFolderAsync(path);
            if (rootDirectory == null)
                return new string[]{"folder not found"};

            var directories = await rootDirectory.GetFoldersAsync();

            return directories.Select(directory => directory.Name).ToArray();
        }

        public async Task<bool> DirectoryExistsAsync(string path)
        {
            StorageFolder folder = await GetFolderAsync(path);
            return folder != null;
        }

        public async Task CreateDirectoryAsync(string path)
        {
            await CreateFolderPathAsync(path);
        }

        public async Task DeleteDirectoryAsync(string path)
        {
            StorageFolder directory = await GetFolderAsync(path);
            if (directory == null)
                return;

            var folders = await directory.GetFoldersAsync();

            foreach (var folder in folders)
            {
                var folderPath = Path.Combine(path, folder.Name);
                await DeleteDirectoryAsync(folderPath);
            }

            foreach (var file in await directory.GetFilesAsync())
            {
                await file.DeleteAsync();
            }

            if (path != "")
                await directory.DeleteAsync();
        }

        public async Task MoveDirectoryAsync(string sourcePath, string destinationPath)
        {
            await CopyDirectoryAsync(sourcePath, destinationPath);

            StorageFolder directory = await GetFolderAsync(sourcePath);
            await directory.DeleteAsync();
        }

        public async Task CopyDirectoryAsync(string sourcePath, string destinationPath)
        {
            await CreateFolderPathAsync(destinationPath);
            StorageFolder directory = await GetFolderAsync(sourcePath);

            if (directory != null)
            {
                foreach (var folder in await directory.GetFoldersAsync())
                {
                    var sourceFolderPath = Path.Combine(sourcePath, folder.Name);
                    var destinationFolderPath = Path.Combine(destinationPath, folder.Name);

                    await CreateDirectoryAsync(destinationFolderPath);
                    await CopyDirectoryAsync(sourceFolderPath, destinationFolderPath);
                }
            }
            var sourceFilePath = "";
            var destinationFilePath = "";

            try
            {
                foreach (var file in await directory.GetFilesAsync())
                {
                    if (file.Name.StartsWith("."))
                        continue;

                    sourceFilePath = Path.Combine(sourcePath, file.Name);
                    destinationFilePath = Path.Combine(destinationPath, file.Name);
                    await CopyFileAsync(sourceFilePath, destinationFilePath);
                }
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Cannot copy {0} to {1}", sourceFilePath, destinationFilePath));
            }
        }

        public async Task RenameDirectoryAsync(string directoryPath, string newDirectoryName)
        {
            StorageFolder directory = await GetFolderAsync(directoryPath);
            if(directory != null)
                await directory.RenameAsync(newDirectoryName);
        }

        #endregion

        #region Specialized reading and writing
        public async Task<object> ReadSerializableObjectAsync(string path, Type type)
        {
            using (var fileStream = await OpenFileAsync(path, StorageFileMode.Open, StorageFileAccess.Read))
            {
                var serializer = new DataContractSerializer(type);

                if (fileStream == null)
                    return null;

                var serializeableObject = serializer.ReadObject(fileStream);
                fileStream.Dispose();
                return serializeableObject;
            }
        }

        public async Task WriteSerializableObjectAsync(string path, object serializableObject)
        {
            using (var fileStream = await OpenFileAsync(path, StorageFileMode.Create, StorageFileAccess.Write))
            {
                var serializer = new DataContractSerializer(serializableObject.GetType());
                serializer.WriteObject(fileStream, serializableObject);
                fileStream.Dispose();
            }
        }

        public async Task<string> ReadTextFileAsync(string path)
        {
            var file = await GetFileAsync(path, false);

            if (file == null)
                return null;

            return await FileIO.ReadTextAsync(file);
        }

        public async Task WriteTextFileAsync(string path, string content)
        {
            var file = await GetFileAsync(path, true);
            await FileIO.WriteTextAsync(file, content);
        }

        #endregion

        #region Image reading and writing
        public async Task<PortableImage> LoadImageAsync(string pathToImage)
        {
            if (await FileExistsAsync(pathToImage))
            {
                try
                {
                    //var bitmapImage = new BitmapImage();

                    //var file = await GetFileAsync(pathToImage);
                    //var stream = await file.OpenAsync(FileAccessMode.Read);
                    //bitmapImage.SetSource(stream);

                    //var writeableBitmap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                    //stream.Seek(0);
                    //writeableBitmap.SetSource(stream);
                    //var portableImage = new PortableImage(writeableBitmap);
                    //// TODO: Dispose Stream?
                    //return portableImage;




                    var bitmapImage = new BitmapImage();

                    var file = await GetFileAsync(pathToImage);
                    var stream = await file.OpenAsync(FileAccessMode.Read);
                    var memoryStream = new MemoryStream();
                    stream.GetInputStreamAt(0).AsStreamForRead().CopyTo(memoryStream);
                    stream.Seek(0);
                    bitmapImage.SetSource(stream);

                    // This is sometimes not working, maybe use BitmapDecoder instead?
                    var writeableBitmap = new WriteableBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    writeableBitmap.SetSource(memoryStream.AsRandomAccessStream());
                    var portableImage = new PortableImage(writeableBitmap)
                    {
                        Width = bitmapImage.PixelWidth,
                        Height = bitmapImage.PixelHeight,
                        EncodedData = memoryStream,
                        ImagePath = pathToImage
                    };
                    stream.Dispose();
                    return portableImage;
                }
                catch (Exception exc)
                {
                    return null;
                }
            }
            return null;
        }

        public async Task SaveImageAsync(string path, PortableImage image, bool deleteExisting, ImageFormat format)
        {
            var withoutExtension = Path.GetFileNameWithoutExtension(path);
            var thumbnailPath = string.Format("{0}{1}", withoutExtension, StorageConstants.ImageThumbnailExtension);

            if (deleteExisting)
            {
                await DeleteFileAsync(path);
                await DeleteFileAsync(thumbnailPath);
            }

            Stream stream = null;

            try
            {
                stream = await OpenFileAsync(path, StorageFileMode.CreateNew, StorageFileAccess.Write);

                switch (format)
                {
                    case ImageFormat.Png:
                        if (image.EncodedData != null)
                        {
                            await image.EncodedData.CopyToAsync(stream);
                        }
                        else
                        {
                            throw new NotImplementedException("This code does not work properly");

                            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(((Stream)image.EncodedData).AsRandomAccessStream());

                            var memoryStream = new InMemoryRandomAccessStream();
                            BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(memoryStream, decoder);

                            try
                            {
                                await encoder.FlushAsync();
                            }
                            catch (Exception exc)
                            {
                                var message = "Error on writing the image: ";

                                if (exc.Message != null)
                                    message += exc.Message;

                                throw new Exception(message);
                            }

                            //await ((WriteableBitmap)image.ImageSource).ToStreamAsJpeg(stream.AsRandomAccessStream());
                            //await PNGWriter.WritePNG((WriteableBitmap)image.ImageSource, stream, 95);
                        }

                        //throw new NotImplementedException();
                        //
                        break;
                    case ImageFormat.Jpg:
                        //await ((WriteableBitmap) image.ImageSource).ToStreamAsJpeg(stream.AsRandomAccessStream());
                        //((WriteableBitmap)image.ImageSource).SaveJpeg(stream, image.Width, image.Height, 0, 95);
                        throw new NotImplementedException();

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("format");
                }
            }
            catch (Exception exc)
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Flush();
                    stream.Dispose();
                }
            }
        }

        public async Task DeleteImageAsync(string pathToImage)
        {
            await DeleteFileAsync(pathToImage);
            await DeleteFileAsync(pathToImage + StorageConstants.ImageThumbnailExtension);
        }

        public async Task<PortableImage> LoadImageThumbnailAsync(string pathToImage)
        {
            //pathToImage = pathToImage.Replace("\\", "/");

            //PortableImage retVal = null;
            //var withoutExtension = Path.GetFileNameWithoutExtension(pathToImage);
            //var imageBasePath = Path.GetDirectoryName(pathToImage);

            //if (imageBasePath != null)
            //{
            //    var thumbnailPath = Path.Combine(imageBasePath, string.Format("{0}{1}",
            //        withoutExtension, StorageConstants.ImageThumbnailExtension));

            //    if (await FileExistsAsync(thumbnailPath))
            //    {
            //        retVal = await LoadImageAsync(thumbnailPath);
            //    }
            //    else
            //    {
            //        var fullSizePortableImage = await LoadImageAsync(pathToImage);

            //        if (fullSizePortableImage != null)
            //        {
            //            var thumbnailImage = await ServiceLocator.ImageResizeService.ResizeImage(
            //                fullSizePortableImage, _imageThumbnailDefaultMaxWidthHeight);
            //            retVal = thumbnailImage;

            //            await thumbnailImage.WriteAsPng(thumbnailPath);
            //        }
            //    }
            //}

            //return retVal;
            return null;
        }

        public async Task TryCreateThumbnailAsync(string filePath)
        {
            //var withoutExtension = Path.GetFileNameWithoutExtension(filePath);
            //var imageBasePath = Path.GetDirectoryName(filePath);

            //var thumbnailPath = Path.Combine(imageBasePath, string.Format("{0}{1}",
            //    withoutExtension, StorageConstants.ImageThumbnailExtension));

            //if (!await FileExistsAsync(thumbnailPath))
            //{
            //    var fullSizePortableImage = await LoadImageAsync(filePath);

            //    if (fullSizePortableImage != null)
            //    {
            //        var thumbnailImage = await ServiceLocator.
            //            ImageResizeService.ResizeImage(fullSizePortableImage,
            //            _imageThumbnailDefaultMaxWidthHeight);

            //        await thumbnailImage.WriteAsPng(thumbnailPath);
            //    }
            //}
        }

        //public async Task<PortableImage> CreateThumbnailAsync(PortableImage image, string imagePath)
        //{
        //    var thumbnailImage = await ServiceLocator.
        //        ImageResizeService.ResizeImage(image,
        //        _imageThumbnailDefaultMaxWidthHeight);

        //    await thumbnailImage.WriteAsPng(imagePath);

        //    return thumbnailImage;
        //}

        #endregion

        #endregion


        public string BasePath
        {
            get { return ""; }
        }

        public void Dispose()
        {
            foreach (var stream in _openedStreams)
            {
                stream.Dispose();
            }
        }

        #region Helpers

        public async Task<StorageFolder> CreateFolderPathAsync(string path)
        {
            if (path == "")
                return _baseFolder;

            try
            {
                StorageFolder folder = await _baseFolder.CreateFolderAsync(path, CreationCollisionOption.OpenIfExists);
                return folder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StorageFolder> GetFolderAsync(string path)
        {
            if (path == "")
                return _baseFolder;

            try
            {
                StorageFolder folder = await _baseFolder.GetFolderAsync(path);
                return folder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StorageFile> GetFileAsync(string path, bool createIfNotExists = true)
        {
            if (path == "")
                return null;

            var fileName = Path.GetFileName(path);
            var directoryName = Path.GetDirectoryName(path);

            StorageFolder folder;
            if (createIfNotExists)
            {
                folder = await CreateFolderPathAsync(directoryName);
            }
            else
            {
                folder = await GetFolderAsync(directoryName);
            }

            if (folder == null)
                return null;
            try
            {
                if (createIfNotExists)
                    return await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

                return await folder.GetFileAsync(fileName);
            }
            catch(Exception)
            {
                return null;
            }
        }


        internal void SetImageMaxThumbnailWidthHeight(int maxWidthHeight)
        {
            _imageThumbnailDefaultMaxWidthHeight = maxWidthHeight;
        }

        #endregion
    }
}