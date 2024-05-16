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

import {
  AccessTokenResponse,
  ForgotPasswordRequest,
  HttpValidationProblemDetails,
  InfoRequest,
  InfoResponse,
  LoginRequest,
  RefreshRequest,
  RegisterRequest,
  ResendConfirmationEmailRequest,
  ResetPasswordRequest,
  TwoFactorRequest,
  TwoFactorResponse,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Identity<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Identity
   * @name RegisterCreate
   * @request POST:/identity/register
   */
  registerCreate = (data: RegisterRequest, params: RequestParams = {}) =>
    this.request<void, HttpValidationProblemDetails>({
      path: `/identity/register`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name LoginCreate
   * @request POST:/identity/login
   */
  loginCreate = (
    data: LoginRequest,
    query?: {
      useCookies?: boolean;
      useSessionCookies?: boolean;
    },
    params: RequestParams = {},
  ) =>
    this.request<AccessTokenResponse, any>({
      path: `/identity/login`,
      method: "POST",
      query: query,
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name RefreshCreate
   * @request POST:/identity/refresh
   */
  refreshCreate = (data: RefreshRequest, params: RequestParams = {}) =>
    this.request<AccessTokenResponse, any>({
      path: `/identity/refresh`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name MapIdentityApiIdentityConfirmEmail
   * @request GET:/identity/confirmEmail
   */
  mapIdentityApiIdentityConfirmEmail = (
    query?: {
      userId?: string;
      code?: string;
      changedEmail?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<void, any>({
      path: `/identity/confirmEmail`,
      method: "GET",
      query: query,
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name ResendConfirmationEmailCreate
   * @request POST:/identity/resendConfirmationEmail
   */
  resendConfirmationEmailCreate = (data: ResendConfirmationEmailRequest, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/identity/resendConfirmationEmail`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name ForgotPasswordCreate
   * @request POST:/identity/forgotPassword
   */
  forgotPasswordCreate = (data: ForgotPasswordRequest, params: RequestParams = {}) =>
    this.request<void, HttpValidationProblemDetails>({
      path: `/identity/forgotPassword`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name ResetPasswordCreate
   * @request POST:/identity/resetPassword
   */
  resetPasswordCreate = (data: ResetPasswordRequest, params: RequestParams = {}) =>
    this.request<void, HttpValidationProblemDetails>({
      path: `/identity/resetPassword`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name Manage2FaCreate
   * @request POST:/identity/manage/2fa
   * @secure
   */
  manage2FaCreate = (data: TwoFactorRequest, params: RequestParams = {}) =>
    this.request<TwoFactorResponse, HttpValidationProblemDetails | void>({
      path: `/identity/manage/2fa`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name ManageInfoList
   * @request GET:/identity/manage/info
   * @secure
   */
  manageInfoList = (params: RequestParams = {}) =>
    this.request<InfoResponse, HttpValidationProblemDetails | void>({
      path: `/identity/manage/info`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Identity
   * @name ManageInfoCreate
   * @request POST:/identity/manage/info
   * @secure
   */
  manageInfoCreate = (data: InfoRequest, params: RequestParams = {}) =>
    this.request<InfoResponse, HttpValidationProblemDetails | void>({
      path: `/identity/manage/info`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
}
