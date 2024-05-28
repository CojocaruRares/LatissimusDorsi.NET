namespace LatissimusDorsi.Server.Data
{
    public class ResourceWrapper<T>
    {
        public T Resource { get; set; }
        public List<Link> Links { get; set; }

        public ResourceWrapper(T data) 
        {
            Resource = data;
            Links = new List<Link>();
        }
    }
}
