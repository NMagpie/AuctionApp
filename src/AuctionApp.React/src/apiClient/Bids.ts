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

import { BidDto, CreateBidCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Bids<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Bids
   * @name BidsDetail
   * @request GET:/Bids/{id}
   */
  bidsDetail = (id: number, params: RequestParams = {}) =>
    this.request<BidDto, any>({
      path: `/Bids/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Bids
   * @name BidsDelete
   * @request DELETE:/Bids/{id}
   * @secure
   */
  bidsDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, void>({
      path: `/Bids/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Bids
   * @name BidsCreate
   * @request POST:/Bids
   * @secure
   */
  bidsCreate = (data: CreateBidCommand, params: RequestParams = {}) =>
    this.request<BidDto, void>({
      path: `/Bids`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
