namespace LatissimusDorsi.Server.Data
{
    public class ResourceWrapper<T>
    {
        public T Data { get; set; }
        public List<Link> Links { get; set; }

        public ResourceWrapper(T data) 
        {
            Data = data;
            Links = new List<Link>();
        }
    }
}
