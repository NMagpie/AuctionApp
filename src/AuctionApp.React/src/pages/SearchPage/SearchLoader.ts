import ApiManager from "../../api/ApiManager";
import { ProductsApiSearchProductsRequest as QueryBody } from "../../api/openapi-generated";

const searchLoader = async (api: ApiManager, request: Request) => {
    const url = new URL(request.url);
    const query = url.searchParams.get("q");
    const category = url.searchParams.get("category");
    const pageIndex = url.searchParams.get("pageIndex");
    const sort = url.searchParams.get("sort")

    let sortField = "";

    let sortDirection = "";

    if (sort) {
        [sortField, sortDirection = "asc"] = sort.split(".");
    }

    const queryBody: QueryBody = {
        pageIndex: pageIndex ?? 0,
        ...query && { searchQuery: query },
        ...category && { category },
        ...sort && {
            columnNameForSorting: sortField,
            sortDirection
        }
    }

    const { data: searchData } = await api.products.searchProducts(queryBody);

    return { searchData, queryBody };
}

export default searchLoader;