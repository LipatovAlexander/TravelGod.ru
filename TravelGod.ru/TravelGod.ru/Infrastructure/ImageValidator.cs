using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace TravelGod.ru.Infrastructure
{
    public static class ImageValidator
    {
        private static readonly string[] PermittedExtensions = {".png", ".jpg", ".jpeg"};

        private static readonly Dictionary<string, List<byte[]>> FileSignature =
            new()
            {
                {
                    ".jpeg", new List<byte[]>
                    {
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE2},
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE3}
                    }
                },
                {
                    ".jpg", new List<byte[]>
                    {
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE1},
                        new byte[] {0xFF, 0xD8, 0xFF, 0xE8}
                    }
                },
                {
                    ".png", new List<byte[]>
                    {
                        new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
                    }
                }
            };


        public static bool IsValid(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !PermittedExtensions.Contains(extension))
            {
                return false;
            }

            if (image.Length > 1024 * 1024) // больше чем мегабайт
            {
                return false;
            }

            using (var reader = new BinaryReader(image.OpenReadStream()))
            {
                var signatures = FileSignature[extension];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                if (signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature)) == false)
                {
                    return false;
                }
            }

            if (!Regex.IsMatch(image.FileName, @"^[\w\-. ]+$"))
            {
                return false;
            }

            return true;
        }
    }
}