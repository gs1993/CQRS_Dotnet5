﻿using CSharpFunctionalExtensions;

namespace Logic.Models
{
    public class Course : Entity
    {
        public virtual string Name { get; protected set; }
        public virtual int Credits { get; protected set; }
    }
}
