import * as signalR from "@microsoft/signalr";
import ApiManager from "../ApiManager";

export default class HttpClient extends signalR.DefaultHttpClient {

    private apiManager: ApiManager;

    constructor(apiManager: ApiManager) {
        super(console);

        this.apiManager = apiManager;
    }
    public async send(
        request: signalR.HttpRequest
    ): Promise<signalR.HttpResponse> {
        const authHeaders = this.getAuthHeaders();
        request.headers = { ...request.headers, ...authHeaders };

        try {
            const response = await super.send(request);
            return response;
        } catch (er) {
            if (er instanceof signalR.HttpError) {
                const error = er as signalR.HttpError;
                if (error.statusCode == 401) {
                    await this.apiManager.refreshAccessToken();
                    const authHeaders = this.getAuthHeaders();
                    request.headers = { ...request.headers, ...authHeaders };
                }
            } else {
                throw er;
            }
        }
        return super.send(request);
    }

    private getAuthHeaders = () => {

        return {
            Authorization: `Bearer ${this.apiManager.userIdentity?.accessToken}`,
        };
    };
}