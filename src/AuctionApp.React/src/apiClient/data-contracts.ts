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

export interface AccessTokenResponse {
  tokenType?: string | null;
  accessToken?: string | null;
  /** @format int64 */
  expiresIn?: number;
  refreshToken?: string | null;
}

export interface AddUserBalanceRequest {
  /** @format double */
  amount?: number;
}

export interface AuctionDto {
  /** @format int32 */
  id?: number;
  title?: string | null;
  /** @format int32 */
  creatorId?: number | null;
  /** @format date-time */
  startTime?: string | null;
  /** @format date-time */
  endTime?: string | null;
  /** @uniqueItems true */
  lotIds?: number[] | null;
}

export interface AuctionReviewDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  userId?: number | null;
  /** @format int32 */
  auctionId?: number;
  reviewText?: string | null;
  /** @format float */
  rating?: number;
}

export interface BidDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  lotId?: number;
  /** @format int32 */
  userId?: number;
  /** @format double */
  amount?: number;
  /** @format date-time */
  createTime?: string;
  isWon?: boolean;
}

export interface CategoryDto {
  /** @format int32 */
  id?: number;
  name?: string | null;
}

export interface CategoryInLotDto {
  name?: string | null;
}

export interface CreateAuctionRequest {
  title?: string | null;
  /** @format date-time */
  startTime?: string;
  /** @format date-time */
  endTime?: string;
  lots?: LotInAuctionDto[] | null;
}

export interface CreateAuctionReviewRequest {
  /** @format int32 */
  auctionId?: number;
  reviewText?: string | null;
  /** @format float */
  rating?: number;
}

export interface CreateBidCommand {
  /** @format int32 */
  lotId?: number;
  /** @format int32 */
  userId?: number;
  /** @format double */
  amount?: number;
}

export interface CreateLotRequest {
  title?: string | null;
  description?: string | null;
  /** @format int32 */
  auctionId?: number;
  /** @format double */
  initialPrice?: number;
  /** @uniqueItems true */
  categories?: string[] | null;
}

export interface CreateUserWatchlistCommand {
  /** @format int32 */
  userId?: number;
  /** @format int32 */
  auctionId?: number;
}

export interface ForgotPasswordRequest {
  email?: string | null;
}

export interface HttpValidationProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  errors?: Record<string, string[]>;
  [key: string]: any;
}

export interface InfoRequest {
  newEmail?: string | null;
  newPassword?: string | null;
  oldPassword?: string | null;
}

export interface InfoResponse {
  email?: string | null;
  isEmailConfirmed?: boolean;
}

export interface LoginRequest {
  email?: string | null;
  password?: string | null;
  twoFactorCode?: string | null;
  twoFactorRecoveryCode?: string | null;
}

export interface LotDto {
  /** @format int32 */
  id?: number;
  title?: string | null;
  description?: string | null;
  /** @format int32 */
  auctionId?: number | null;
  /** @format double */
  initialPrice?: number | null;
  /** @uniqueItems true */
  bidIds?: number[] | null;
  /** @uniqueItems true */
  categories?: CategoryDto[] | null;
}

export interface LotInAuctionDto {
  title?: string | null;
  description?: string | null;
  /** @format double */
  initialPrice?: number;
  /** @uniqueItems true */
  categories?: CategoryInLotDto[] | null;
}

export interface RefreshRequest {
  refreshToken?: string | null;
}

export interface RegisterRequest {
  email?: string | null;
  password?: string | null;
}

export interface ResendConfirmationEmailRequest {
  email?: string | null;
}

export interface ResetPasswordRequest {
  email?: string | null;
  resetCode?: string | null;
  newPassword?: string | null;
}

export interface TwoFactorRequest {
  enable?: boolean | null;
  twoFactorCode?: string | null;
  resetSharedKey?: boolean;
  resetRecoveryCodes?: boolean;
  forgetMachine?: boolean;
}

export interface TwoFactorResponse {
  sharedKey?: string | null;
  /** @format int32 */
  recoveryCodesLeft?: number;
  recoveryCodes?: string[] | null;
  isTwoFactorEnabled?: boolean;
  isMachineRemembered?: boolean;
}

export interface UpdateAuctionRequest {
  title?: string | null;
  /** @format date-time */
  startTime?: string;
  /** @format date-time */
  endTime?: string;
}

export interface UpdateAuctionReviewRequest {
  reviewText?: string | null;
  /** @format float */
  rating?: number;
}

export interface UpdateLotRequest {
  title?: string | null;
  description?: string | null;
  /** @format double */
  initialPrice?: number;
  /** @uniqueItems true */
  categories?: string[] | null;
}

export interface UpdateUserRequest {
  userName?: string | null;
  email?: string | null;
  password?: string | null;
}

export interface UserDto {
  /** @format int32 */
  id?: number;
  userName?: string | null;
  /** @format double */
  balance?: number;
}

export interface UserWatchlistDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  userId?: number;
  /** @format int32 */
  auctionId?: number | null;
}
