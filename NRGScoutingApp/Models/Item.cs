using System;

namespace NRGScoutingApp
{
    [Preserve(AllMembers = true)]
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}