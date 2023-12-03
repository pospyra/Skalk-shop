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

        public static string FindOffersByIdsQuery = @"
        query FindOffersByIds($itemIds: [ID!]!) {
          supSearch(filters: { id: $itemIds }) {
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
                    name
                  }
                  offers(filter: { id: { in: $itemIds } }) {
                    id
                    clickUrl
                    inventoryLevel
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


    }
}
