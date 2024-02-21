using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Language : Entity
{
    public byte LanguageId { get; set; }

    public string Name { get; set; } = null!;
}
