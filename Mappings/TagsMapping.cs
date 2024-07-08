using WhatToEat.DTOs;
using WhatToEat.Entities;

namespace WhatToEat.Mappings
{
    public static class TagsMapping
    {
        public static TagCreator ToDto(this Tag tag)
        {
            return new TagCreator(tag.Id, tag.Text);
        }
    }
}
