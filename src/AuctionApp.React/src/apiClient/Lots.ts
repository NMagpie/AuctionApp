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

import { CreateLotRequest, LotDto, UpdateLotRequest } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Lots<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Lots
   * @name LotsDetail
   * @request GET:/Lots/{id}
   */
  lotsDetail = (id: number, params: RequestParams = {}) =>
    this.request<LotDto, any>({
      path: `/Lots/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Lots
   * @name LotsUpdate
   * @request PUT:/Lots/{id}
   * @secure
   */
  lotsUpdate = (id: number, data: UpdateLotRequest, params: RequestParams = {}) =>
    this.request<LotDto, void>({
      path: `/Lots/${id}`,
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
   * @tags Lots
   * @name LotsDelete
   * @request DELETE:/Lots/{id}
   * @secure
   */
  lotsDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, void>({
      path: `/Lots/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Lots
   * @name LotsCreate
   * @request POST:/Lots
   * @secure
   */
  lotsCreate = (data: CreateLotRequest, params: RequestParams = {}) =>
    this.request<LotDto, void>({
      path: `/Lots`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
