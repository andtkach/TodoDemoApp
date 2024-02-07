import React from 'react';
import Button from './Button';
import "../index.css";

const Header = ({ showForm, changeTextAndColor, showSettings }) => {
    return (
        <header className="header">
            <h2 className="app-header">Task Manager App</h2>
            <div className="d-flex justify-content-end">
                <Button onClick={showForm} color={changeTextAndColor ? 'red' : 'green'} text={changeTextAndColor ? 'Close' : 'Add'} />
                <Button onClick={showSettings} color={'gray'} text={'#'} />
            </div>
        </header>
    )
}

export default Header;
