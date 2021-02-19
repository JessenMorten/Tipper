import React from 'react';
import { NavLink, Route } from 'react-router-dom';
import { Home } from "./components/Home";
import { About } from "./components/About";
import { TipBuilder } from "./components/TipBuilder";

export const App = () => {
    return (
        <div className="container py-5">
            <header className="text-center">
                <NavLink to="/" className="text-decoration-none">
                    <h1>Tipper</h1>
                </NavLink>
                <hr />
            </header>

            <Route exact path='/' component={Home} />
            <Route exact path='/om' component={About} />
            <Route exact path='/tip' component={TipBuilder} />
        </div>
    );
};
