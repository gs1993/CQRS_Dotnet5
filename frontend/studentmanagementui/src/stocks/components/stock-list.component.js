import React, { useMemo, useState, useEffect } from "react";
import axios from "axios";
import Table from "./Table";

const Genres = ({ values }) => {
    // Loop through the array and create a badge-like component instead of a comma-separated string
    return (
        <>
            {values.map((genre, idx) => {
                return (
                    <span key={idx} className="badge">
                        {genre}
                    </span>
                );
            })}
        </>
    );
};

const Runtime = ({ value }) => {
    const hour = Math.floor(value / 60);
    const min = Math.floor(value % 60);
    return (
        <>
            {hour > 0 ? `${hour} hr${hour > 1 ? "s" : ""} ` : ""}
            {min > 0 ? `${min} min${min > 1 ? "s" : ""}` : ""}
        </>
    );
};


export default function StockList() {
    const columns = useMemo(
        () => [
            {
                // first group - TV Show
                Header: "TV Show",
                // First group columns
                columns: [
                    {
                        Header: "Name",
                        accessor: "show.name"
                    },
                    {
                        Header: "Type",
                        accessor: "show.type"
                    }
                ]
            },
            {
                // Second group - Details
                Header: "Details",
                // Second group columns
                columns: [
                    {
                        Header: "Language",
                        accessor: "show.language"
                    },
                    {
                        Header: "Genre(s)",
                        accessor: "show.genres",
                        Cell: ({ cell: { value } }) => <Genres values={value} />
                    },
                    {
                        Header: "Runtime",
                        accessor: "show.runtime",
                        Cell: ({ cell: { value } }) => <Runtime value={value} />
                    },
                    {
                        Header: "Status",
                        accessor: "show.status"
                    }
                ]
            }
        ],
        []
    );

    // data state to store the TV Maze API data. Its initial value is an empty array
    const [data, setData] = useState([]);

    // Using useEffect to call the API once mounted and set the data
    useEffect(() => {
        (async () => {
            const result = await axios("https://api.tvmaze.com/search/shows?q=snow");
            setData(result.data);
        })();
    }, []);

    return (
        <div>
            <Table columns={columns} data={data} />
        </div>
    );


}


