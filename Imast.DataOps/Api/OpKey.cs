namespace Imast.DataOps.Api
{
    /// <summary>
    /// The operation key
    /// </summary>
    public readonly struct OpKey
    {
        /// <summary>
        /// The operation group
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// The operation name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates new instance of operation key
        /// </summary>
        /// <param name="group">The group name</param>
        /// <param name="name">The operation name</param>
        public OpKey(string group, string name) : this()
        {
            this.Group = group ?? string.Empty;
            this.Name = name ?? string.Empty;
        }

        /// <summary>
        /// Creates new instance of operation key
        /// </summary>
        /// <param name="name">The operation name</param>
        public OpKey(string name) : this()
        {
            this.Group = string.Empty;
            this.Name = name ?? string.Empty;
        }

        /// <summary>
        /// Creates new instance of operation key
        /// </summary>
        /// <param name="name">The operation name</param>
        public static OpKey Of(string name)
        {
            return new OpKey(name);
        }

        /// <summary>
        /// Creates new instance of operation key
        /// </summary>
        /// <param name="group">The group name</param>
        /// <param name="name">The operation name</param>
        public static OpKey Of(string group, string name)
        {
            return new OpKey(group, name);
        }

        /// <summary>
        /// Checks if this key is equal to given one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // should not be null
            if (obj is OpKey key)
            {
                return string.Equals(this.Name, key.Name) && string.Equals(this.Group, key.Group);
            }

            return false;
        }

        /// <summary>
        /// Calculate the hashcode of key
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return 31 * this.Name.GetHashCode() + this.Group.GetHashCode();
            }
        }
    }
}