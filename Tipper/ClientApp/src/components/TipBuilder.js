import React from 'react';

export const TipBuilder = () => {
    const [isLoadingChannels, setIsLoadingChannels] = React.useState(false);
    const [channels, setChannels] = React.useState();
    const [tipChannel, setTipChannel] = React.useState();
    const [tipTime, setTipTime] = React.useState(new Date());

    const formatTipTime = (date) => {
        const dateFormat = new Intl.DateTimeFormat('da-DK', { weekday: 'long', month: 'long', day: 'numeric' });
        const timeFormat = new Intl.DateTimeFormat('da-DK', { hour: 'numeric', minute: 'numeric' });
        return dateFormat.format(date) + " kl. " + timeFormat.format(date);
    }

    if (!isLoadingChannels && !channels) {
        fetch("televisionschedule/channels")
            .then(r => r.json())
            .then(r => { setChannels(r); setIsLoadingChannels(false); });
        setIsLoadingChannels(true);
    }

    if (isLoadingChannels || !channels) {
        return (
            <div className="text-center my-5">
                <div className="spinner-grow text-primary" role="status"></div>
            </div>
        );
    }

    if (!tipChannel) {
        return (
            <>
                <h4 className="text-center my-3">Vælg kanal</h4>
                <input className="form-control rounded-pill text-center" placeholder="S�g kanaler..." />
                <div className="row row-cols-2 row-cols-md-3 row-cols-lg-4">
                    {channels.map(c => (
                        <button key={c.id} className="btn col p-3" onClick={() => setTipChannel(c)}>
                            <div className="border p-1 shadow-sm rounded-lg text-center">
                                <img alt={c.title} title={c.title} style={{ width: "100%", maxHeight: "100px" }} src={`data:image/svg+xml;base64,${btoa(c.svgLogo)}`} />
                            </div>
                        </button>
                    ))}
                </div>
            </>);
    }

    const Tip = () => {
        const recipient = "?????@???.??";
        const subject = `${tipChannel.title} ${formatTipTime(tipTime)}.`;
        const body = `${tipChannel.title} ${formatTipTime(tipTime)}.`;
        const mailto = `mailto:${recipient}?subject=${encodeURI(subject)}&body=${encodeURI(body)}`;

        return (
            <>
                <p>Kære Natholdet, jeg har et tip!</p>
                <p>Jeg så noget sjovt på {tipChannel.title} {formatTipTime(tipTime)}.</p>
                <p>{formatTipTime(tipTime)}</p>
                <a
                    className="btn btn-primary p-3 mt-5 rounded-pill shadow mx-auto col-10 col-md-6 d-block"
                    href={mailto}><h5 className="m-0">Send tip</h5></a>
            </>);
    };

    return (
        <div className="text-center my-3">
            <img alt={tipChannel.title} title={tipChannel.title} style={{ width: "100%", maxHeight: "100px" }} src={`data:image/svg+xml;base64,${btoa(tipChannel.svgLogo)}`} />
            <Tip />
            <button
                className="btn btn-secondary p-3 mt-5 rounded-pill shadow mx-auto col-10 col-md-6 d-block"
                onClick={() => setTipChannel(undefined)}>Vælg anden kanal</button>
        </div>
    );
};
