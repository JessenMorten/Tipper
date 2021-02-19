import React from 'react';
import { NavLink } from 'react-router-dom';

export const Home = () => {
    const BigLink = ({ text, to, color }) => (
        <NavLink to={to} className={`btn btn-${color} btn-block p-3 mt-5 rounded-pill shadow`}>
            <h5 className="m-0">{text}</h5>
        </NavLink>
    );

    return (
        <div className="text-center mx-auto col-10 col-md-6">
            <BigLink to="/tip" color="primary" text="Jeg har et tip!" />
            <BigLink to="/om" color="outline-secondary" text="Hvad er Tipper?" />
        </div>
    );
};
