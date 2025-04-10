﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManagement.Service.Extencions
{
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly string[] formats = new[]
        {
            "yyyy", "yyyy-MM", "yyyy-MM-dd",
            "yyyy/MM/dd", "yyyy.MM.dd", "yyyyMMdd",
            "dd-MM-yyyy", "dd/MM/yyyy", "dd.MM.yyyy" 
        };

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string dateString = reader.GetString()?.Trim();
                if (string.IsNullOrWhiteSpace(dateString))
                    return null;

                Console.WriteLine($"Parsing date string: {dateString}");

                // Убираем .0 перед цифрами
                dateString = Regex.Replace(dateString, @"\.0(\d)", ".$1");

                // Если есть символ ~, берем первую дату
                if (dateString.Contains("~"))
                {
                    dateString = dateString.Split('~')[0].Trim();
                }

                // Если введен только год (например, "2018"), добавляем "-01-01"
                if (Regex.IsMatch(dateString, @"^\d{4}$"))
                {
                    dateString += "-01-01";  // Преобразуем "2018" → "2018-01-01"
                }

                // Если введен только год и месяц (например, "2018-05"), добавляем "-01"
                if (Regex.IsMatch(dateString, @"^\d{4}-\d{1,2}$"))
                {
                    dateString += "-01";  // Преобразуем "2018-05" → "2018-05-01"
                }

                if (DateTime.TryParseExact(dateString, formats, new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }

                // Дополнительная попытка парсинга конкретно для dd.MM.yyyy
                if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }

                // Попробуем без строгого формата
                if (DateTime.TryParse(dateString, out date))
                {
                    return date;
                }
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            Console.WriteLine($"Failed to parse date: {reader.GetString()}");
            throw new JsonException($"Invalid date format: {reader.GetString()}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
