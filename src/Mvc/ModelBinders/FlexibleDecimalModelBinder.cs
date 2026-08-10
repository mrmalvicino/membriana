using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace Mvc.ModelBinders;

public class FlexibleDecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

        var rawValue = valueResult.FirstValue;

        if (string.IsNullOrWhiteSpace(rawValue))
        {
            if (bindingContext.ModelMetadata.IsReferenceOrNullableType)
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }

            return Task.CompletedTask;
        }

        if (TryParseDecimal(rawValue, out var amount))
        {
            bindingContext.Result = ModelBindingResult.Success(amount);
            return Task.CompletedTask;
        }

        bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "El valor debe ser un número válido.");

        return Task.CompletedTask;
    }

    private static bool TryParseDecimal(string rawValue, out decimal amount)
    {
        var value = rawValue.Trim();
        var styles = NumberStyles.Number;

        var lastCommaIndex = value.LastIndexOf(',');
        var lastDotIndex = value.LastIndexOf('.');

        if (lastCommaIndex >= 0 && lastDotIndex >= 0)
        {
            var decimalSeparator = lastCommaIndex > lastDotIndex ? ',' : '.';
            var thousandsSeparator = decimalSeparator == ',' ? '.' : ',';
            var normalizedValue = value.Replace(thousandsSeparator.ToString(), string.Empty)
                .Replace(decimalSeparator.ToString(), CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            return TryParse(normalizedValue, CultureInfo.InvariantCulture, styles, out amount);
        }

        if (lastCommaIndex >= 0 || lastDotIndex >= 0)
        {
            var separatorIndex = Math.Max(lastCommaIndex, lastDotIndex);
            var fractionalDigits = value.Length - separatorIndex - 1;

            if (fractionalDigits is > 0 and <= 2)
            {
                var normalizedValue = value.Replace(',', '.');
                return TryParse(normalizedValue, CultureInfo.InvariantCulture, styles, out amount);
            }
        }

        if (TryParse(value, CultureInfo.CurrentCulture, styles, out amount))
        {
            return true;
        }

        if (TryParse(value, CultureInfo.InvariantCulture, styles, out amount))
        {
            return true;
        }

        return false;
    }

    private static bool TryParse(string value, CultureInfo culture, NumberStyles styles, out decimal amount)
    {
        return decimal.TryParse(value, styles, culture, out amount);
    }
}
