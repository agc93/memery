using System;
using System.Linq;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Memery.Configuration;
using System.IO;
using System.Collections.Generic;

namespace Memery
{
    public static class CoreExtensions
    {
        public static Guid ToGuid(this string src)
        {
            byte[] stringbytes = System.Text.Encoding.UTF8.GetBytes(src);
            byte[] hashedBytes = new System.Security.Cryptography
                .SHA1CryptoServiceProvider()
                .ComputeHash(stringbytes);
            Array.Resize(ref hashedBytes, 16);
            return new Guid(hashedBytes);
        }

        public static bool ContainsAny(this string s, params char[] chars)
        {
            return s.Any(c => chars.Contains(c));
        }

        public static ImageResponse ToResponse(this ImageReference imgRef)
        {
            //var fn = System.IO.Path.GetRelativePath(System.IO.Directory.GetCurrentDirectory(), imgRef.Location.FullName);
            //var ir = new ImageResponse(imgRef.Id, imgRef.Name, imgRef.Location);
            //return ir;
            return imgRef as ImageResponse;
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            // note: creating a Random instance each call may not be correct for you,
            // consider a thread-safe static instance
            var r = new Random();
            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
        }

        internal static string ToDirectoryPath(this string s) {
            return new DirectoryInfo(s).FullName;
        }

        internal static string ToFilePath(this string s) {
            return new FileInfo(s).FullName;
        }
    }
}