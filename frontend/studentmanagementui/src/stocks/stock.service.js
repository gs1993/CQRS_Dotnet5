import http from "../http-common";

class StockDataService {
    getAll() {
        return http.get("/StockData")
            .then(function (response) {
                console.log(response);
                return response;
            });
    }

}

export default new StockDataService();