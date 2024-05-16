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

import { AddUserBalanceRequest, UpdateUserRequest, UserDto } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Users<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Users
   * @name UsersDetail
   * @request GET:/Users/{id}
   */
  usersDetail = (id: number, params: RequestParams = {}) =>
    this.request<UserDto, any>({
      path: `/Users/${id}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Users
   * @name UsersUpdate
   * @request PUT:/Users/{id}
   * @secure
   */
  usersUpdate = (id: number, data: UpdateUserRequest, params: RequestParams = {}) =>
    this.request<UserDto, void>({
      path: `/Users/${id}`,
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
   * @tags Users
   * @name UsersDelete
   * @request DELETE:/Users/{id}
   * @secure
   */
  usersDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, void>({
      path: `/Users/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Users
   * @name AddBalanceCreate
   * @request POST:/Users/add-balance
   * @secure
   */
  addBalanceCreate = (data: AddUserBalanceRequest, params: RequestParams = {}) =>
    this.request<void, void>({
      path: `/Users/add-balance`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
}
