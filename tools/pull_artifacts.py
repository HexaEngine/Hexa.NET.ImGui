import os
import requests

# Configuration
TOKEN_FILE = 'token.txt'  # File containing the personal access token
REPO_OWNER = 'HexaEngine'  
REPO_NAME = 'Hexa.NET.ImGui' 
BASE_URL = f'https://api.github.com/repos/{REPO_OWNER}/{REPO_NAME}'

# Set up headers for API requests
headers = {
    'Accept': 'application/vnd.github.v3+json',
}

# Define the workflows you want to target
TARGET_WORKFLOWS = [
    'Build all Libraries (Android)',
    'Build cimgui Libraries',
    'Build cimguizmo Libraries',
    'Build cimnodes Libraries',
    'Build cimplot Libraries',
]

def read_token_from_file(token_file):
    """Read the personal access token from a file."""
    with open(token_file, 'r') as file:
        return file.read().strip()

def get_workflow_id(workflow_name):
    """Get the workflow ID based on the workflow name."""
    url = f'{BASE_URL}/actions/workflows'
    response = requests.get(url, headers=headers)
    response.raise_for_status()  # Raise an error for bad responses
    workflows = response.json().get('workflows', [])
    
    # Find the workflow ID based on the name
    for workflow in workflows:
        if workflow['name'] == workflow_name:
            return workflow['id']
    return None

def get_latest_workflow_run(workflow_id):
    """Get the latest workflow run for a specified workflow ID."""
    url = f'{BASE_URL}/actions/workflows/{workflow_id}/runs?per_page=1'
    response = requests.get(url, headers=headers)
    response.raise_for_status()  # Raise an error for bad responses
    runs = response.json().get('workflow_runs', [])
    return runs[0] if runs else None  # Return the most recent run or None if no runs exist

def get_artifacts_for_run(run_id):
    """Get artifacts for a specified run ID."""
    url = f'{BASE_URL}/actions/runs/{run_id}/artifacts'
    response = requests.get(url, headers=headers)
    response.raise_for_status()
    return response.json().get('artifacts', [])

def download_artifact(artifact_url, artifact_name):
    """Download the artifact from the given URL."""
    response = requests.get(artifact_url, headers=headers)
    response.raise_for_status()
    with open(artifact_name + ".zip", 'wb') as f:
        f.write(response.content)

def main():
    # Read the GitHub token from the file
    GITHUB_TOKEN = read_token_from_file(TOKEN_FILE)

    # Update headers with the token
    headers['Authorization'] = f'token {GITHUB_TOKEN}'

    for workflow_name in TARGET_WORKFLOWS:
        print(f'Checking workflow: {workflow_name}')
        
        # Get the workflow ID for the specified workflow name
        workflow_id = get_workflow_id(workflow_name)
        if not workflow_id:
            print(f'  Workflow not found: {workflow_name}')
            continue
        
        # Get the latest run for the workflow
        latest_run = get_latest_workflow_run(workflow_id)
        if not latest_run:
            print(f'  No runs found for workflow: {workflow_name}')
            continue
        
        run_id = latest_run['id']
        print(f'  Latest run ID: {run_id}')
        
        # Get artifacts for the latest run
        artifacts = get_artifacts_for_run(run_id)
        for artifact in artifacts:
            # Use the original artifact name
            artifact_name = artifact['name']  # Keep the original name
            download_url = artifact['archive_download_url']
            print(f'    Downloading artifact: {artifact_name}')
            download_artifact(download_url, artifact_name)

if __name__ == '__main__':
    main()
