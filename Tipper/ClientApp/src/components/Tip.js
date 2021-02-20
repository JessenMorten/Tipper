import React from 'react';

export const Tip = ({ channelTitle, tipTime, programTitle }) => {
    const recipient = "natholdet@tv2.dk";
    const subject = `Jeg har et tip!`;
    const body = `Kære Natholdet, jeg har et tip! Jeg så "${programTitle}" på ${channelTitle}, ${tipTime}...`;
    const mailto = `mailto:${recipient}?subject=${encodeURI(subject)}&body=${encodeURI(body)}`;

    return (
        <>
            <p>{body}</p>
            <a
                className="btn btn-primary p-3 mt-5 rounded-pill shadow mx-auto col-10 col-md-6 d-block"
                href={mailto}><h5 className="m-0">Send tip</h5></a>
        </>);
};
