﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2mobi
{
    public class Book(string title, string author, ReadOnlyCollection<Book.Section> sects)
    {
        public class Section(int sectNum, string title, ReadOnlyCollection<string> paras)
        {
            public readonly int SectionNumber = sectNum;
            public readonly string Title = title;
            public readonly ReadOnlyCollection<string> Paragraphs = paras;
        }

        public readonly string Title = title;
        public readonly string Author = author;
        public readonly ReadOnlyCollection<Section> Sections = sects;
    }
}
