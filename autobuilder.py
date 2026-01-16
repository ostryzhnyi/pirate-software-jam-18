# builds unity webgl build and uploads it to itch
# you'll need an itch.io butler for this
# https://itch.io/docs/butler/login.html
  
import os
import subprocess
import time
from git import Repo

# Configuration
BRANCH = 'origin/main'
REPO_PATH = '/Users/ostrzhnyi/Work/pirate-software-jam-18'  # Path to the local Git repository
UNITY_PROJECT_PATH = REPO_PATH  # Path to the Unity project
UNITY_EXECUTABLE = '/Applications/Unity/Hub/Editor/6000.3.4f1/Unity.app/Contents/MacOS/Unity'  # Path to the Unity executable
WEBGL_BUILD_PATH = REPO_PATH+'/webgl_build'  # Path to store WebGL build
BUTLER_EXECUTABLE = '/Users/ostrzhnyi/Work/cicd-gem/bin/butler'  # Path to the butler executable (assuming you're running the script from the butler directory)
                                # you can read about butler here https://itch.io/docs/butler/login.html
ITCH_PROJECT = 'ostryzhnyi/test:webgl'  # Itch.io project and channel

CHECK_INTERVAL = 60  # Time in seconds to wait before checking for new commits


def get_latest_commit_hash(repo_path):
    """Gets the latest commit hash from the git repository."""
    repo = Repo(repo_path)
    return repo.head.commit.hexsha

def pull_latest_changes(repo_path):
    """Pulls the latest changes from the remote repository."""
    repo = Repo(repo_path)
    repo.git.reset('--hard', BRANCH)
    origin = repo.remotes.origin
    origin.pull()

def build_unity_project():
    """Triggers a headless WebGL build for the Unity project."""
    try:
        # Build the single command string as you would run it in the terminal
        unity_command = (
            f'{UNITY_EXECUTABLE} -batchmode -nographics -quit '
            f'-projectPath "{UNITY_PROJECT_PATH}" '
            f'-executeMethod jam.CodeBase.WebGLBuilder.Build  '
            f'-buildTarget WebGL '
            f'-output "{WEBGL_BUILD_PATH}"'
        )
        
        subprocess.run(unity_command, shell=True, check=True)
        print("Unity build completed successfully.")

        # # Upload build to Itch.io using Butler
        print("Uploading build to Itch.io...")
        subprocess.run([
            BUTLER_EXECUTABLE,
            'push',
            WEBGL_BUILD_PATH,  # Path to WebGL build
            ITCH_PROJECT  # Itch.io project and channel
        ], check=True)
        print("Build uploaded to Itch.io successfully.")

    except subprocess.CalledProcessError as e:
        print(f"Unity build failed: {e}")

def main():
    last_commit_hash = get_latest_commit_hash(REPO_PATH)
    print(f"Initial commit hash: {last_commit_hash}")

    # Ensure the build path exists
    if not os.path.exists(WEBGL_BUILD_PATH):
        os.makedirs(WEBGL_BUILD_PATH)

    while True:
        # Pull the latest changes
        pull_latest_changes(REPO_PATH)

        current_commit_hash = get_latest_commit_hash(REPO_PATH)
        if current_commit_hash != last_commit_hash:
            print(f"New commit detected: {current_commit_hash}")
            last_commit_hash = current_commit_hash
            build_unity_project()
        else:
            print("No new commits found.")

        time.sleep(CHECK_INTERVAL)

if __name__ == '__main__':
    main()

#
# you will also need to implement a build script inside of unity, you can start with this one
#
# public class WebGLBuilder
# {
#    [MenuItem("Build/Build WebGL")]
#    public static void Build()
#    {
#        string[] scenes =
#        {
#            "Assets/main.unity", 
#        };
#
#        PlayerSettings.defaultWebScreenWidth = 1340;
#        PlayerSettings.defaultWebScreenHeight = 710;
#
#        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
#        buildPlayerOptions.scenes = scenes;
#        buildPlayerOptions.locationPathName = "webgl_build";
#        buildPlayerOptions.target = BuildTarget.WebGL;
#        buildPlayerOptions.options = BuildOptions.None;
#        
#        BuildPipeline.BuildPlayer(scenes, "webgl_build", BuildTarget.WebGL, BuildOptions.None);
#    }
# }
