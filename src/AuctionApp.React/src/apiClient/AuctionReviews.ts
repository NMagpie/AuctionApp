/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import { AuctionReviewDto, CreateAuctionReviewRequest, UpdateAuctionReviewRequest } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class AuctionReviews<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags AuctionReviews
   * @name AuctionReviewsDetail
   * @request GET:/AuctionReviews/{id}
   */
  auctionReviewsDetail = (id: number, params: RequestParams = {}) =>
    this.request<AuctionReviewDto, any>({
      path: `/AuctionReviews/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags AuctionReviews
   * @name AuctionReviewsUpdate
   * @request PUT:/AuctionReviews/{id}
   * @secure
   */
  auctionReviewsUpdate = (id: number, data: UpdateAuctionReviewRequest, params: RequestParams = {}) =>
    this.request<AuctionReviewDto, void>({
      path: `/AuctionReviews/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags AuctionReviews
   * @name AuctionReviewsDelete
   * @request DELETE:/AuctionReviews/{id}
   * @secure
   */
  auctionReviewsDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, void>({
      path: `/AuctionReviews/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags AuctionReviews
   * @name AuctionReviewsCreate
   * @request POST:/AuctionReviews
   * @secure
   */
  auctionReviewsCreate = (data: CreateAuctionReviewRequest, params: RequestParams = {}) =>
    this.request<AuctionReviewDto, void>({
      path: `/AuctionReviews`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
