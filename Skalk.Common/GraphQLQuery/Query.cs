namespace Skalk.Common.GraphQLQuery
{
    public static class Query
    {
        public static string FindPricesQuery = @"
        query FindPrices($itemName: String!) {
            supSearch(q: $itemName, limit: 1) { 
                results {
                    part {
                        id
                        name
                        mpn
                        manufacturer {
                            name
                        }
                        sellers(includeBrokers: false) {
                            company {
                                id
                                name
                            }
                            offers {
                                id
                                clickUrl
                                inventoryLevel
                                moq
                                prices {
                                    price
                                    currency
                                    quantity
                                }
                            }
                        }
                    }
                }
            }
        }
    ";

        public static string FindProductByItemCart = @"
             query MultiMatch($itemName: String!)  {
              supMultiMatch(
                queries: [{mpn: $itemName, limit: 1}]
                options: {filters: {distributor_id: ""2402""}}) {
                hits
                parts {
                        id
                        name
                        mpn
                        manufacturer {
                            name
                        }
                        sellers(includeBrokers: false) {
                            company {
                                id
                                name
                            }
                            offers {
                                id
                                clickUrl
                                inventoryLevel
                                moq
                                prices {
                                    price
                                    currency
                                    quantity
                                }
                            }
                        }
                    }
                }
              }
    ";


    }
}
