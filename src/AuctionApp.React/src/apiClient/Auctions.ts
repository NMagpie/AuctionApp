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

import { AuctionDto, CreateAuctionRequest, UpdateAuctionRequest } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Auctions<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Auctions
   * @name AuctionsDetail
   * @request GET:/Auctions/{id}
   */
  auctionsDetail = (id: number, params: RequestParams = {}) =>
    this.request<AuctionDto, any>({
      path: `/Auctions/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Auctions
   * @name AuctionsUpdate
   * @request PUT:/Auctions/{id}
   * @secure
   */
  auctionsUpdate = (id: number, data: UpdateAuctionRequest, params: RequestParams = {}) =>
    this.request<AuctionDto, void>({
      path: `/Auctions/${id}`,
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
   * @tags Auctions
   * @name AuctionsDelete
   * @request DELETE:/Auctions/{id}
   * @secure
   */
  auctionsDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, void>({
      path: `/Auctions/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Auctions
   * @name AuctionsCreate
   * @request POST:/Auctions
   * @secure
   */
  auctionsCreate = (data: CreateAuctionRequest, params: RequestParams = {}) =>
    this.request<AuctionDto, void>({
      path: `/Auctions`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
