using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace school_major_project.GlobalServices
{
    public class TrimModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                var value = valueProviderResult.FirstValue;
                if (!string.IsNullOrEmpty(value))
                {
                    bindingContext.Result = ModelBindingResult.Success(value.Trim());
                }
            }

            return Task.CompletedTask;
        }
    }
}
