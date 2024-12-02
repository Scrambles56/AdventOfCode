const readline = require("readline");
const { stdin, stdout } = require("process");

main()
  .then(() => {})
  .catch((err) => {
    console.error(err);
    process.exit(1);
  });

async function main() {
  var session = process.env.AOC_SESSION;
  if (!session) {
    const rl = readline.createInterface({ input: stdin, output: stdout });

    session = await new Promise((resolve) => {
      rl.question(
        "Please enter your Advent of Code session cookie: ",
        (answer) => {
          resolve(answer);
        }
      );
    });

    rl.close();
  }

  if (!session) {
    console.error("No session cookie provided.");
    process.exit(1);
  }

  const url = "https://adventofcode.com/2024/day/2/input";
  const headers = { cookie: `session=${session}` };

  var resp = await fetch(url, { headers });
  if (!resp.ok) {
    console.error("Failed to fetch input:", resp.statusText);
    process.exit(1);
  }

  let input = await resp.text();

  let lines = input
    .trim()
    .split("\n")
    .map((line) => line.trim())
    .filter((line) => line.length > 0);

  let valid = 0;
  for (const line of lines) {
    let numbers = line.split(" ").map((n) => parseInt(n));

    for (let i = -1; i < numbers.length; i++) {
      var numbersCopy = [...numbers];
      if (i >= 0) {
        numbersCopy.splice(i, 1);
      }

      var prev = null;
      var mode = null; // -1: descending, 1: ascending
      var invalidCount = 0;
      lineValid = true;

      for (let i = 0; i < numbersCopy.length && lineValid; i++) {
        current = numbersCopy[i];

        if (prev !== null && mode === null) {
          mode = current > prev ? 1 : -1;
        }

        if (!isValid(current, prev, mode)) {
          lineValid = false;
          break;
        }

        prev = current;
      }

      if (lineValid) {
        valid++;
        break;
      }
    }
  }

  console.log(valid);
}

function isValid(current, prev, mode) {
  if (mode === null) {
    return true;
  }

  var diff = (current - prev) * mode;

  return 0 < diff && diff <= 3;
}
