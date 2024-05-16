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

import { CreateUserWatchlistCommand, UserWatchlistDto } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class UserWatchlists<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags UserWatchlists
   * @name UserWatchlistsDetail
   * @request GET:/UserWatchlists/{id}
   */
  userWatchlistsDetail = (id: number, params: RequestParams = {}) =>
    this.request<UserWatchlistDto, any>({
      path: `/UserWatchlists/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags UserWatchlists
   * @name UserWatchlistsDelete
   * @request DELETE:/UserWatchlists/{id}
   * @secure
   */
  userWatchlistsDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, void>({
      path: `/UserWatchlists/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags UserWatchlists
   * @name UserWatchlistsCreate
   * @request POST:/UserWatchlists
   * @secure
   */
  userWatchlistsCreate = (data: CreateUserWatchlistCommand, params: RequestParams = {}) =>
    this.request<UserWatchlistDto, void>({
      path: `/UserWatchlists`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
