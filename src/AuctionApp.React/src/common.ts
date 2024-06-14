import { UserDto, BidDto, ProductDto } from "./api/openapi-generated";

export const hasMore = (index: number, pageSize: number, total: number) => (index + 1) * pageSize < total;

export type Product = {
    id: number;
    title: string;
    description: string;
    creator: UserDto | null;
    initialPrice: number;
    startTime: Date | null;
    endTime: Date | null;
    bids: Set<BidDto>;
};

export const productDtoToProduct = (productDto: ProductDto) => {
    const product: Product = {
        id: productDto.id,
        title: productDto.title ?? '',
        description: productDto.description ?? '',
        creator: productDto.creator ?? null,
        initialPrice: productDto.initialPrice,
        startTime: productDto.startTime ? new Date(productDto.startTime) : null,
        endTime: productDto.endTime ? new Date(productDto.endTime) : null,
        bids: productDto.bids ?? [],
    };

    return product;
};

export const dateFormat = { year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' };

export type ProductStatus = "pending" | "started" | "finished";

export const getProductStatus = (product: Product): ProductStatus => {

    if (!product) {
        return null;
    }

    const currentDate = new Date();

    const isSellingStarted = product?.startTime <= currentDate;

    const isSellingEnded = product?.endTime <= currentDate;

    switch (true) {
        case (!isSellingStarted && !isSellingEnded):
            return "pending"
        case (isSellingStarted && !isSellingEnded):
            return "started"
        case (isSellingStarted && isSellingEnded):
            return "finished";
    }
};

export const getSearchQuery = (query: QueryBody) => {
    const params: Array<string> = [];

    query.pageIndex && params.push(`pageIndex=${query.pageIndex}`);

    query.searchQuery && params.push(`q=${query.searchQuery}`);

    query.category && params.push(`category=${query.category}`);

    query.columnNameForSorting && params.push(`sort=
        ${query.columnNameForSorting}
        ${query.sortDirection && `.${query.sortDirection}`
        }`);

    const preset = query.searchPreset ? `preset=${query.searchPreset}` : "preset=none";

    params.push(preset);

    query.minPrice && params.push(`minPrice=${query.minPrice}`);

    query.maxPrice && params.push(`maxPrice=${query.maxPrice}`);

    return "/search?" + params.join("&");
};