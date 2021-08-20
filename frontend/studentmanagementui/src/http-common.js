import axios from "axios";

export default axios.create({
    baseURL: "https://api.aletheiaapi.com",
    headers: {
        "Content-type": "application/json",
        "key": "9542E7E2C1D54916B666929AA22D6270"
    }
});