using System;
using FluentValidation.Validators;
using NzbDrone.Core.Tv;

namespace NzbDrone.Core.Validation.Paths
{
    public class BookExistsValidator : PropertyValidator
    {
        private readonly ISeriesService _seriesService;

        public BookExistsValidator(ISeriesService seriesService)
            : base("This book has already been added")
        {
            _seriesService = seriesService;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return true;

            var googleID = context.PropertyValue.ToString();
            return (!_seriesService.GetAllBooks().Exists(s => s.GoogleID == googleID));
        }
    }
}
