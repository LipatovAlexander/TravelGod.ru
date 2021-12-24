using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TravelGod.ru.Infrastructure.Validation.Interfaces;

namespace TravelGod.ru.Infrastructure.Validation
{
    public class ImageValidator : IValidator<FormFile>
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

        public IEnumerable<ModelValidationResult> Validate(FormFile instance)
        {
            var extension = Path.GetExtension(instance.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !PermittedExtensions.Contains(extension))
            {
                yield return new ModelValidationResult(nameof(instance), "Недопустимое расширение у изображения.");
                yield break;
            }

            if (instance.Length > 1024 * 1024) // больше чем мегабайт
            {
                yield return new ModelValidationResult(nameof(instance), "Файл не должен весить больше 1 мб.");
                yield break;
            }

            using (var reader = new BinaryReader(instance.OpenReadStream()))
            {
                var signatures = FileSignature[extension];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                if (signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature)) == false)
                {
                    yield return new ModelValidationResult(nameof(instance), "Некорректный файл.");
                    yield break;
                }
            }

            if (!Regex.IsMatch(instance.FileName, @"^[\w\-. ]+$"))
            {
                yield return new ModelValidationResult(nameof(instance), "Недопустимое название файла.");
            }
        }
    }
}