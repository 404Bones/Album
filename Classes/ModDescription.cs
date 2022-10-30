namespace Album
{
    public class ModDescription
    {
        public ModDescription(string modname, string desc)
        {
            Album.moddesc.Add(modname.ToLower(), desc);
        }
    }
}
