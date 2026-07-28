import { Link } from "react-router-dom";

export default function NotFoundPage() {
  return (
    <div className="container py-5 text-center">
      <h1>404</h1>
      <p>The requested page was not found.</p>
      <Link className="primary-button d-inline-block" to="/">
        Go Home
      </Link>
    </div>
  );
}
