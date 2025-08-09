#!/bin/bash

echo "Cleaning up development folder..."

find src/development -type d -name "bin" -exec rm -rf {} +
find src/development -type d -name "obj" -exec rm -rf {} +
find src/development -type d -name ".vs" -exec rm -rf {} +
find src/development -type d -name "node_modules" -exec rm -rf {} +

echo "Cleanup complete."
