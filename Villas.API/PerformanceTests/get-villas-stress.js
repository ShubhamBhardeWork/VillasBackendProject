import http from 'k6/http';
import { check } from 'k6';

export const options = {
  stages: [
    { duration: '20s', target: 10 },
    { duration: '20s', target: 25 },
    { duration: '20s', target: 50 },
    { duration: '20s', target: 100 },
    { duration: '20s', target: 0 },
  ],
};

export default function () {
  const res = http.get('https://localhost:8000/api/villas');

check(res, {
    'GET /api/villas status is 200': (r) => r.status === 200,
  });
}