import React from 'react';
import { Tip } from "./Tip";

export const TipBuilder = () => {
    // Loading state
    const [isLoadingChannels, setIsLoadingChannels] = React.useState(false);
    const [isLoadingProgramDescriptions, setIsLoadingProgramDescriptions] = React.useState(false);

    const [channels, setChannels] = React.useState();
    const [tipChannel, setTipChannel] = React.useState();
    const [tipTime, setTipTime] = React.useState(new Date());
    const [programDescriptions, setProgramDescriptions] = React.useState([]);

    const getCurrentProgramTitle = () => {
        const nowUnix = new Date().getTime() / 1000;
        for (let i = 0; i < programDescriptions.length; i++) {
            let item = programDescriptions[i];

            if (item.startTimeUnix <= nowUnix && item.stopTimeUnix >= nowUnix) {
                return item.title;
            }
        }

        return "Reklamer?";
    };

    const loadProgramDescription = (channelId) => {
        fetch(`televisionschedule/program/${channelId}/${new Date().toUTCString()}`)
            .then(r => r.json())
            .then(r => { setProgramDescriptions(r); setIsLoadingProgramDescriptions(false); });
        setIsLoadingProgramDescriptions(true);
    };

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
                <input className="form-control rounded-pill text-center" placeholder="Søg kanaler..." />
                <div className="row row-cols-2 row-cols-md-3 row-cols-lg-4">
                    {channels.map(c => (
                        <button key={c.id} className="btn col p-3" onClick={() => {
                            setTipChannel(c);
                            loadProgramDescription(c.id);
                        }}>
                            <div className="border p-1 shadow-sm rounded-lg text-center">
                                <img alt={c.title} title={c.title} style={{ width: "100%", maxHeight: "100px" }} src={`data:image/svg+xml;base64,${btoa(c.svgLogo)}`} />
                            </div>
                        </button>
                    ))}
                </div>
            </>);
    }

    return (
        <div className="text-center my-3">
            <img alt={tipChannel.title} title={tipChannel.title} style={{ width: "100%", maxHeight: "100px" }} src={`data:image/svg+xml;base64,${btoa(tipChannel.svgLogo)}`} />
            <Tip
                programTitle={getCurrentProgramTitle()}
                tipTime={formatTipTime(tipTime)}
                channelTitle={tipChannel.title} />
            <button
                className="btn btn-outline-secondary p-3 mt-5 rounded-pill shadow mx-auto col-10 col-md-6 d-block"
                onClick={() => setTipChannel(undefined)}><h5 className="m-0">Vælg anden kanal</h5></button>
        </div>
    );
};
