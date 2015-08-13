using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Data
{
	public class EditablePackageFile1 : FileInfoBase
	{
		
		//public bool IsIncluded { get; set; }

		//public static List<EditablePackageFile> Convert(List<FileInfo> fileInfoList)
		//{
		//    List<Data.EditablePackageFile> filesInPackage = new List<Data.EditablePackageFile>();

		//    foreach (FileInfo fileInfo in fileInfoList)
		//    {
		//        string fileName = fileInfo.Name;
		//        bool includedInPackage = true;
		//        if (fileName.EndsWith(AutoUpdateManager.ExcludedMarker) == true)
		//        {
		//            fileName = fileName.Remove(fileName.Length - AutoUpdateManager.ExcludedMarker.Length);
		//            includedInPackage = false;
		//        }

		//        filesInPackage.Add(new Data.EditablePackageFile()
		//        {
		//            DateCreated = Format.DateTime(fileInfo.CreationTime),
		//            Name = fileName,
		//            IsIncluded = includedInPackage,
		//            LastModified = Format.DateTime(fileInfo.LastWriteTime)
		//        });
		//    }
		//}
	}
}
