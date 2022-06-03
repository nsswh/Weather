import React, { Component } from 'react';
import { Container } from 'reactstrap';
import './custom.css';
import catinsnow from './catinsnow.jpeg'
import catinsfreezingrain from './catinfreezingrain.jpg'
import warmdog from './warmdog.jpeg'
import wetcat from './wetcat.jpeg'
import catinice from './catinice.jpeg'

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true, city: '', days: '', url: '', juststarting: true };
        this.handleChange = this.handleChange.bind(this)
        this.handleSearchSubmit = this.handleSearchSubmit.bind(this)
    }

    // if we want to display a default city weather when the page is loaded the 1st time, enable this method
    //componentDidMount() {
    //    this.populateWeatherData();
    //}

    // only when the submit button is clicked(the url state is not empty), load data from backend
    componentDidUpdate() {
        if (this.state.url != '') {
            this.populateWeatherData();
        }
    }

    // Search button is clicked 
    handleSearchSubmit = (event) => {
        const { city, days } = this.state;
        if (this.state.juststarting) {
            this.setState({
                juststarting: false
            })
        }
        // compose url { api/skys/Vancouver } or { api/skys/Vancouver/3 }
        let url = 'api/skys/';
        if (city != '') {
            url = url + city;
        }
        if (days != '') {
            if (/^[0-9]+$/.test(days)) {
                if (days != '0') {
                    url = url + '/' + days;
                }
            }
        }
        // the state url change will trigger data loading from the backend
        this.setState({
            url: url, city: '', days: ''
        })
        event.preventDefault();
    };

    handleChange(event) {
        this.setState({
            [event.target.name]: event.target.value
        })
    }

    // conditional rendering of the cat or dog images according to weather condition
    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>City</th>
                        <th>Day</th>
                        <th>Temp. (C)</th>
                        <th>Precipitation</th>
                        <th>Weather Condition</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.Skyid}>
                            <td>{forecast.city}</td>
                            <td>{forecast.day}</td>
                            <td>{forecast.temp}</td>
                            <td>{forecast.preciptype}</td>
                            <td><img alt="timer" src={
                                forecast.preciptype == 'None' ? warmdog :
                                    (forecast.preciptype == 'Ice' ? catinice :
                                        (forecast.preciptype == 'Freezingrain' ? catinsfreezingrain :
                                            (forecast.preciptype == 'Snow' ? catinsnow : wetcat)))
                            } /></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    // conditional rendering of welcome message when the app starts, or no matching result message when no results from the backend
    render() {
        let contents = this.state.juststarting ? <div className="mx-auto text-info"><h3>Welcome to the weather forecast, please type a city name to start</h3></div> :
            (this.state.loading ? <div className="mx-auto"><p><em>Loading...</em></p></div> :
                (this.state.forecasts.length == 0 ? <div className="mx-auto text-warning"><h3>Sorry there is no city name match your input, please try again</h3></div> :
                    App.renderForecastsTable(this.state.forecasts)));

        return (
            <Container>
                <div className="container-fluid">
                    <div className="row">
                        <div className="col bg-dark text-white text-center">
                            <div className="navbar-brand ">Weather Forecast</div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="form-group mx-auto m-1">
                            <form onSubmit={this.handleSearchSubmit} >
                                <input className="m-1"
                                    name="city"
                                    type="text"
                                    placeholder="City..."
                                    value={this.state.city}
                                    onChange={this.handleChange}
                                />
                                <input className="m-1"
                                    name="days"
                                    type="text"
                                    placeholder="15 days..."
                                    value={this.state.days}
                                    onChange={this.handleChange}
                                />
                                <button className="m-1">Search</button>
                            </form>
                        </div>
                    </div>
                    <div className="row">
                        {contents}
                    </div>
                </div>
            </Container>
        );
    }

    async populateWeatherData() {
        const response = await fetch(this.state.url);
        const data = await response.json();
        // after data loaded from the backend, set the state url to be empty string so that componentDidUpdate() won't load the data from the backend again
        this.setState({ forecasts: data, loading: false, url: '' });
    }
}
