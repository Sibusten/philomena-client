using System;

namespace Philomena.Client
{
    public class PhilomenaClient : IPhilomenaClient
    {
        public ISearchQuery Search(string query)
        {
            throw new NotImplementedException();
        }

        ISearchQuery IPhilomenaClient.Search()
        {
            throw new NotImplementedException();
        }
    }
}
