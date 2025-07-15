import {MyAuthInfo} from "./my-auth-info";

export interface LoginTokenDto {
  myAuthInfo: MyAuthInfo | null;
  token: string;
}
