﻿namespace tomenglertde.ProjectConfigurationManager.View
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;

    using TomsToolbox.Core;

    /// <summary>
    /// Interaction logic for MultipleChoiceFilter.xaml
    /// </summary>
    public class MultipleChoiceTagFilter : MultipleChoiceFilterBase
    {
        internal static readonly Regex Regex = new Regex(@"\W+", RegexOptions.Compiled);

        static MultipleChoiceTagFilter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultipleChoiceTagFilter), new FrameworkPropertyMetadata(typeof(MultipleChoiceFilterBase)));
        }

        protected override void OnSourceValuesChanged(IEnumerable<string> newValue)
        {
            if (newValue == null)
                Values.Clear();
            else
                Values.SynchronizeWith(new[] { string.Empty }.Concat(newValue.SelectMany(x => Regex.Split(x))).Distinct().ToArray());
        }

        protected override MultipleChoiceContentFilterBase CreateFilter(IEnumerable<string> items)
        {
            return new TagsContentFilter(items);
        }

        private class TagsContentFilter : MultipleChoiceContentFilterBase
        {
            public TagsContentFilter(IEnumerable<string> items)
                : base(items)
            {
            }

            public override bool IsMatch(object value)
            {
                var input = value as string;
                if (string.IsNullOrWhiteSpace(input))
                    return Items?.Contains(string.Empty) ?? true;

                var tags = Regex.Split(input);

                return Items?.ContainsAny(tags) ?? true;
            }
        }
    }
}
