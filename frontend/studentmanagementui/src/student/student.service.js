import http from "../http-common";

class StudentDataService {
    getAll() {
        return http.get("/students")
            .then(function (response) {
                console.log(response);
                if (response.errorMessage)
                    return response.errorMessage;
                return response.result;
            });
    }

    // get(id) {
    //     return http.get(`/students/${id}`);
    // }

    create(data) {
        return http.post("/students", data);
    }

    update(id, data) {
        return http.put(`/students/${id}`, data);
    }

    delete(id) {
        return http.delete(`/students/${id}`);
    }

    // findByTitle(title) {
    //     return http.get(`/students?title=${title}`);
    // }
}

export default new StudentDataService();