import './main-page.css';
import Header from "./header";
import { BrowserRouter as Router, Switch, Route, Link } from "react-router-dom"
import AddStudent from '../student/components/add-student.component'
import StockList from "../stocks/components/stock-list.component";

function App() {
  return (
    <Router>
      <div className="container">
        <Header subtitle="Student management APP" />
      </div>

      <nav className="navbar navbar-expand navbar-dark bg-dark">
        <a href="/students" className="navbar-brand">
          Main page
        </a>
        <div className="navbar-nav mr-auto">
          <li className="nav-item">
            <Link to={"/students"} className="nav-link">
              Students
            </Link>
          </li>
          <li className="nav-item">
            <Link to={"/add"} className="nav-link">
              Enroll student
            </Link>
          </li>
          <li className="nav-item">
            <Link to={"/stocks"} className="nav-link">
              Stocks
            </Link>
          </li>
        </div>
      </nav>
      <div className="container mt-3">
        <Switch>
          {/* <Route exact path={["/", "/students"]} component={StudentList} /> */}
          <Route exact path="/add" component={AddStudent} />
          <Route exact path="/stocks" component={StockList} />
          {/* <Route path="/students/:id" component={Student} /> */}
        </Switch>
      </div>
    </Router>
  );
}

export default App;
